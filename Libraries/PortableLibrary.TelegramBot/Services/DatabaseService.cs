using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Data.Database.Entities;

namespace PortableLibrary.TelegramBot.Services
{
    public class DatabaseService
    {
        private BotDataContext _context;

        public DatabaseService(BotDataContext context)
        {
            _context = context;
        }

        public async Task AddSequenceCommandAsync(long chatId, string command, string language, string parentCommand = null)
        {
            var state = await _context.ChatCommandSequencesState.FirstOrDefaultAsync(s => s.ChatId == chatId && s.Command == command);
            if (state != null)
                throw new Exception("Command has already been added to the sequence.");

            state = new ChatCommandSequenceState()
            {
                ChatId = chatId,
                Command = command,
                Language = language,
                CreationDate = DateTime.Now
            };

            if (!string.IsNullOrWhiteSpace(parentCommand))
            {
                var parent = await _context.ChatCommandSequencesState.FirstOrDefaultAsync(s => s.ChatId == chatId && s.Command == parentCommand);
                state.ParentChatCommandSequenceState = parent;
            }

            await _context.ChatCommandSequencesState.AddAsync(state);
        }

        public async Task<ChatCommandSequenceState> GetFirstCommand(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderBy(s => s.CreationDate).FirstOrDefaultAsync();
            return sequence;
        }

        public async Task<ChatCommandSequenceState> GetLastCommand(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderByDescending(s => s.CreationDate).FirstOrDefaultAsync();
            return sequence;
        }

        public async Task<List<ChatCommandSequenceState>> GetCommandSequence(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderBy(s => s.CreationDate).ToListAsync();
            return sequence;
        }

        public async Task<string> GetCommandSequenceKey(long chatId)
        {
            var sequence = await GetCommandSequence(chatId);
            if (sequence.Count > 0)
                return string.Join(".", sequence.Select(s => s.Command));
            return null;
        }

        public async Task<bool> ClearSequence(long chatId, int? sequenceLength = null)
        {
            var sequence = await GetCommandSequence(chatId);
            if (sequenceLength.HasValue ? sequence.Count == sequenceLength.Value : true)
            {
                _context.ChatCommandSequencesState.RemoveRange(sequence.Where(s => s.ChatId == chatId));
                return true;
            }
            return false;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
